using UnityEngine;

public class LevelBlock : MonoBehaviour
{
    [SerializeField] private GameObject belowTorch;
    [SerializeField] private GameObject aboveTorch;
    [SerializeField] private GameObject leftTorch;
    [SerializeField] private GameObject rightTorch;
    public void DetermineTexture(TileMap map, Tile info)
    {
        Renderer renderer = this.GetComponent<Renderer>();
        switch (info.ReturnNeighborBlocks())
        {
            case Direction.None:
                renderer.materials[2].SetTexture("_MainTex", map.None);
                this.DetermineTorch(info, Direction.None);
                break;
            case Direction.Left:
                renderer.materials[2].SetTexture("_MainTex", map.Left);
                this.DetermineTorch(info, Direction.Left);
                break;
            case Direction.Right:
                renderer.materials[2].SetTexture("_MainTex", map.Right);
                this.DetermineTorch(info, Direction.Right);
                break;
            case Direction.Below:
                renderer.materials[2].SetTexture("_MainTex", map.Below);
                this.DetermineTorch(info, Direction.Below);
                break;
            case Direction.Above:
                renderer.materials[2].SetTexture("_MainTex", map.Above);
                this.DetermineTorch(info, Direction.Above);
                break;
            case Direction.BelowLeft:
                renderer.materials[2].SetTexture("_MainTex", map.BelowLeft);
                this.DetermineTorch(info, Direction.BelowLeft);
                break;
            case Direction.BelowRight:
                renderer.materials[2].SetTexture("_MainTex", map.BelowRight);
                this.DetermineTorch(info, Direction.BelowRight);
                break;
            case Direction.AboveRight:
                renderer.materials[2].SetTexture("_MainTex", map.AboveRight);
                this.DetermineTorch(info, Direction.AboveRight);
                break;
            case Direction.AboveLeft:
                renderer.materials[2].SetTexture("_MainTex", map.AboveLeft);
                this.DetermineTorch(info, Direction.AboveLeft);
                break;
            case Direction.LeftRight:
                renderer.materials[2].SetTexture("_MainTex", map.LeftRight);
                this.DetermineTorch(info, Direction.LeftRight);
                break;
            case Direction.AboveBelow:
                renderer.materials[2].SetTexture("_MainTex", map.AboveBelow);
                this.DetermineTorch(info, Direction.AboveBelow);
                break;
            case Direction.AboveBelowLeft:
                renderer.materials[2].SetTexture("_MainTex", map.AboveBelowLeft);
                this.DetermineTorch(info, Direction.AboveBelowLeft);
                break;
            case Direction.AboveBelowRight:
                renderer.materials[2].SetTexture("_MainTex", map.AboveBelowRight);
                this.DetermineTorch(info, Direction.AboveBelowRight);
                break;
            case Direction.AboveLeftRight:
                renderer.materials[2].SetTexture("_MainTex", map.AboveLeftRight);
                this.DetermineTorch(info, Direction.AboveLeftRight);
                break;
            case Direction.BelowLeftRight:
                renderer.materials[2].SetTexture("_MainTex", map.BelowLeftRight);
                this.DetermineTorch(info, Direction.BelowLeftRight);
                break;
            case Direction.AboveBelowLeftRight:
                renderer.materials[2].SetTexture("_MainTex", map.All);
                if (DetermineNumberOfTripletNeighbors(info) <= 2)
                {
                    this.transform.localScale = this.transform.localScale + new Vector3(0.3f, 0.01f, 0.3f);
                }
                break;
        }
    }

    private int DetermineNumberOfTripletNeighbors(Tile info)
    {
        int sum = 0;

        foreach (Tile n in info.neighbors)
        {
            Direction d = n.ReturnNeighborBlocks();
            if (d != Direction.AboveBelowLeftRight)
            {
                sum++;
            }
        }

        return sum;
    }

    private void DetermineTorch(Tile info, Direction dir)
    {
        if (info.torchChance >= GlobalRandom.RandomInt(100))
        {
            switch (dir)
            {
                case Direction.None:
                    belowTorch.SetActive(true);
                    aboveTorch.SetActive(true);
                    leftTorch.SetActive(true);
                    rightTorch.SetActive(true);
                    break;
                case Direction.Left:
                    rightTorch.SetActive(true);
                    belowTorch.SetActive(true);
                    leftTorch.SetActive(true);
                    break;
                case Direction.Right:
                    rightTorch.SetActive(true);
                    aboveTorch.SetActive(true);
                    leftTorch.SetActive(true);
                    break;
                case Direction.Below:
                    aboveTorch.SetActive(true);
                    belowTorch.SetActive(true);
                    rightTorch.SetActive(true);
                    break;
                case Direction.Above:
                    belowTorch.SetActive(true);
                    leftTorch.SetActive(true);
                    rightTorch.SetActive(true);
                    break;
                case Direction.BelowLeft:
                    aboveTorch.SetActive(true);
                    rightTorch.SetActive(true);
                    break;
                case Direction.BelowRight:
                    aboveTorch.SetActive(true);
                    leftTorch.SetActive(true);
                    break;
                case Direction.AboveRight:
                    belowTorch.SetActive(true);
                    leftTorch.SetActive(true);
                    break;
                case Direction.AboveLeft:
                    rightTorch.SetActive(true);
                    belowTorch.SetActive(true);
                    break;
                case Direction.LeftRight:
                    leftTorch.SetActive(true);
                    rightTorch.SetActive(true);
                    break;
                case Direction.AboveBelow:
                    aboveTorch.SetActive(true);
                    belowTorch.SetActive(true);
                    break;
                case Direction.AboveBelowLeft:
                    aboveTorch.SetActive(true);
                    break;
                case Direction.AboveBelowRight:
                    aboveTorch.SetActive(true);
                    break;
                case Direction.AboveLeftRight:
                    leftTorch.SetActive(true);
                    break;
                case Direction.BelowLeftRight:
                    rightTorch.SetActive(true);
                    break;
                case Direction.AboveBelowLeftRight:
                    break;
            }
        }
    }
}
