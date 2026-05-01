using UnityEngine;
using MyGameNamespace;

public class SoldierGroupCommander : MonoBehaviour
{
    public Transform player;

    void Update()
    {
        // Grup 0
        if (Input.GetKeyDown(KeyCode.Z))
            CommandGroup(0, true); // Follow
        if (Input.GetKeyDown(KeyCode.X))
            CommandGroup(0, false); // Attack

        // Grup 1
        if (Input.GetKeyDown(KeyCode.C))
            CommandGroup(1, true);
        if (Input.GetKeyDown(KeyCode.V))
            CommandGroup(1, false);
    }

    void CommandGroup(int groupID, bool follow)
    {
        foreach (SoldierController s in FindObjectsOfType<SoldierController>())
        {
            if (s.groupID != groupID) continue;

            if (follow)
                s.FollowMe(player);
            else
            {
                s.StopFollowing();
                s.canAttack = true;
                s.FindNewTarget();
            }
        }
    }
}
