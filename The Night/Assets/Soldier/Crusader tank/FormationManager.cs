// FormationManager.cs

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using MyGameNamespace;

public enum FormationType { Rectangle, Circle, Triangle }

public class FormationManager : MonoBehaviour
{
    public float spacing = 2f;
    public FormationType currentFormation = FormationType.Rectangle;

    public void ArrangeFormation(List<GameObject> soldiers, Vector3 center)
    {
        // Dezactivează temporar AI-ul
        List<SoldierController> controllers = new List<SoldierController>();
        foreach (GameObject soldier in soldiers)
        {
            var controller = soldier.GetComponent<SoldierController>();
            if (controller != null)
            {
                controller.enabled = false;
                if (controller.Animator != null)
                    controller.Animator.SetBool("merge", false);
                controllers.Add(controller);
            }
        }

        // Alege formația
        // Alege formația
        switch (currentFormation)
        {
            case FormationType.Rectangle:
                ArrangeRectangle(soldiers, center);
                break;
            case FormationType.Circle:
                ArrangeCircle(soldiers, center);
                break;
            case FormationType.Triangle:
                ArrangeTriangle(soldiers, center);
                break;
        }

        // Aplică buff-urile în funcție de formație
        ApplyFormationBuffs(soldiers);

        // Reactivare AI după repoziționare
        foreach (var controller in controllers)
            controller.StartCoroutine(ReactivateSoldier(controller, 0.1f));

    }
    public void ApplyFormationBuffs(List<GameObject> soldiers)
    {
        foreach (var soldier in soldiers)
        {
            var stats = soldier.GetComponent<SoldierStats>();
            if (stats == null) continue;

            switch (currentFormation)
            {
                case FormationType.Rectangle:
                    stats.defenseMultiplier = 1.2f;
                    stats.attackMultiplier = 1.0f;
                    stats.regenRate = 0f;
                    break;

                case FormationType.Circle:
                    stats.defenseMultiplier = 1.0f;
                    stats.attackMultiplier = 1.0f;
                    stats.regenRate = 2f; // HP per second
                    break;

                case FormationType.Triangle:
                    stats.defenseMultiplier = 0.9f;
                    stats.attackMultiplier = 1.5f;
                    stats.regenRate = 0f;
                    break;
            }
        }
    }

    IEnumerator ReactivateSoldier(SoldierController controller, float delay)
    {
        yield return new WaitForSeconds(delay);
        controller.enabled = true;
    }

    void ArrangeRectangle(List<GameObject> soldiers, Vector3 center)
    {
        int rowSize = Mathf.CeilToInt(Mathf.Sqrt(soldiers.Count));
        for (int i = 0; i < soldiers.Count; i++)
        {
            int row = i / rowSize;
            int col = i % rowSize;
            Vector3 pos = center + new Vector3(col * spacing, 0, row * spacing);
            MoveSoldier(soldiers[i], pos);
        }
    }

    void ArrangeCircle(List<GameObject> soldiers, Vector3 center)
    {
        float radius = spacing + soldiers.Count * 0.2f;
        for (int i = 0; i < soldiers.Count; i++)
        {
            float angle = i * Mathf.PI * 2f / soldiers.Count;
            Vector3 pos = center + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
            MoveSoldier(soldiers[i], pos);
        }
    }

    void ArrangeTriangle(List<GameObject> soldiers, Vector3 center)
    {
        int row = 0, placed = 0;
        while (placed < soldiers.Count)
        {
            int rowCount = row + 1;
            for (int i = 0; i < rowCount && placed < soldiers.Count; i++)
            {
                Vector3 offset = new Vector3(i * spacing - row * spacing / 2f, 0, row * spacing);
                MoveSoldier(soldiers[placed], center + offset);
                placed++;
            }
            row++;
        }
    }

    void MoveSoldier(GameObject soldier, Vector3 position)
    {
        // Dacă există NavMeshAgent, folosește Warp pentru a evita conflicte
        var agent = soldier.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.Warp(position);
            return;
        }
        // Altfel, dacă e RigidBody
        var rb = soldier.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.MovePosition(position);
        }
        else
        {
            soldier.transform.position = position;
        }
    }
}