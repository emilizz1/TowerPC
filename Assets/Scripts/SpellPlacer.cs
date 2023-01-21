using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SpellPlacer
{
    public static GameObject spellToPlace;

    public static List<Spell> activeSpells = new List<Spell>();

    static internal bool spellPlaced;

    public static IEnumerator PlaceSpell()
    {
        Camera camera = Camera.main;
        Transform spellTransform = spellToPlace.transform;
        Spell mySpell = spellToPlace.GetComponent<Spell>();
        int snapping = spellToPlace.GetComponent<Spell>().snappingSize;
        Vector3 newPos = Vector3.zero;
        Plane plane = new Plane(Vector3.up, 0f);
        Ray ray;
        float distance;
        while (spellToPlace != null)
        {
            ray = camera.ScreenPointToRay(Input.mousePosition);
            if (plane.Raycast(ray, out distance))
            {
                newPos = ray.GetPoint(distance);
                newPos.x = Mathf.Round( newPos.x / snapping) * snapping;
                newPos.z = Mathf.Round((newPos.z- 0.5f) / snapping) * snapping;
                if(spellTransform.position != newPos)
                {
                    spellTransform.position = newPos;
                    mySpell.SpellMoved();
                }
                else
                {
                    spellTransform.position = newPos;
                }
            }
            yield return null;
        }
    }

    public static void SpellPlaced()
    {
        if(activeSpells == null)
        {
            activeSpells = new List<Spell>();
        }
        Spell spell = spellToPlace.GetComponent<Spell>();
        spell.Activate();
        activeSpells.Add(spell);
        spellPlaced = true;
    }

    public static void StopAllSpells()
    {
        List<Spell> spellsToRemove = new List<Spell>();
        if (activeSpells != null)
        {
            foreach (Spell spell in activeSpells)
            {
                spell.StopSpell();
                if(spell == null || spell.duration == 0)
                {
                    spellsToRemove.Add(spell);
                }
            }

            foreach(Spell removeSpell in spellsToRemove)
            {
                activeSpells.Remove(removeSpell);
            }
        }
    }

    public static void Reset()
    {
        spellToPlace = null;
        activeSpells = new List<Spell>();
        spellPlaced = false;
    }
}
