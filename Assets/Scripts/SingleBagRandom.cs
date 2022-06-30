using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SingleBagRandom : RandomSelector
{
    string[] tetrominoBag = { "T", "O", "Z", "S", "I", "L", "J" };

    List<string> randomOrder = new List<string>(7);
    List<string> pieceBag = new List<string> { "T", "O", "Z", "S", "I", "L", "J" };

    public override string GetNextTetromino()
    {
        if(randomOrder.Count == 0)
        {
            if (pieceBag.Count == 0)
                pieceBag = tetrominoBag.ToList();

            while (pieceBag.Count > 0)
            {
                string randomPiece = pieceBag[Random.Range(0, pieceBag.Count)];
                pieceBag.Remove(randomPiece);
                randomOrder.Add(randomPiece);
            }
        }

        string piece = randomOrder[Random.Range(0, randomOrder.Count)];
        randomOrder.Remove(piece);
        return piece;
    }
}
