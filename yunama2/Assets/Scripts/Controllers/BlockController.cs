using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    public int energyAmount;
    public int magicAmount;

    public Sprite currentSprite;
    public SpriteRenderer spriteRenderer;

    [SerializeField] Sprite emptyBlock;
    [SerializeField] Sprite[] energyBlock =  new Sprite[4];
    [SerializeField] Sprite[] magicBlock =  new Sprite[4];
     
    void Start() {
        getBlockSpritesFromResources();
    }

    void Update() {
        setBlockSprite(isEnergyBlock(), getLevel());
    }

    public void changeEnergy(int energy){
        // 足したり引いたりしたあとにSpriteを変更する必要があるか確認するために事前に値を保管
        bool beforeIsEnergy = isEnergyBlock();
        int beforeLevel = getLevel();
        
        energyAmount += energy;

        bool currentIsEnergy = isEnergyBlock();
        int currentLevel = getLevel();

        if (beforeIsEnergy != currentIsEnergy || beforeLevel != currentLevel) {
            setBlockSprite(currentIsEnergy, currentLevel);
        }
    }

    public void changeMagic(int magic){
        // 足したり引いたりしたあとにSpriteを変更する必要があるか確認するために事前に値を保管
        bool beforeIsEnergy = isEnergyBlock();
        int beforeLevel = getLevel();
        
        magicAmount += magic;

        bool currentIsEnergy = isEnergyBlock();
        int currentLevel = getLevel();

        if (beforeIsEnergy != currentIsEnergy || beforeLevel != currentLevel) {
            setBlockSprite(currentIsEnergy, currentLevel);
        }
    }

    public int getAmount() {
        return Mathf.Max(energyAmount, magicAmount);
    }

    public bool isEnergyBlock() {
        // 同じなら養分
        return energyAmount >= magicAmount;
    }

    public bool isMagicBlock() {
        return magicAmount >= energyAmount;
    }

    public int[] getPostition() {
        return new int[] { (int)(transform.position.x - 0.5f), (int)(transform.position.y - 0.5f) };
    }

    public int getLevel() {
        // lv0 0
        // lv1 1 - 9
        // lv2 10 - 29
        // lv3 30 -
        if (energyAmount == 0 && magicAmount == 0) 
            return 0;
        else if(Mathf.Max(energyAmount, magicAmount) <= 9)
            return 1;
        else if(Mathf.Max(energyAmount, magicAmount) <= 29)
            return 2;
        else 
            return 3;
    }

    void setBlockSprite(bool isEnergy, int level) {
        if (level == 0) {
            changeSprite(emptyBlock);
        } else if(isEnergy) {
            changeSprite(energyBlock[level]);
        } else {
            changeSprite(magicBlock[level]);
        }
    }

    void getBlockSpritesFromResources(){
        // 養分レベル0 は 一応Emptyを入れておく
        emptyBlock = Resources.Load<Sprite>("EmptyBlocks/emptyBlock");
        energyBlock[0] = Resources.Load<Sprite>("EmptyBlocks/emptyBlock");
        magicBlock[0] = Resources.Load<Sprite>("EmptyBlocks/emptyBlock");
        for(int i=1; i<=3; i++) {
            energyBlock[i] = Resources.Load<Sprite>($"EnergyBlocks/energy_{i}");
            magicBlock[i] = Resources.Load<Sprite>($"MagicBlocks/magic_{i}");
        }
    }

    public void initalizeBlock(GameObject block, int randomEmptyRange, int randomEnergyAmountRange, int y, int x) {
        BlockController blockController = block.GetComponent<BlockController>();
        bool isEmpty = Random.Range(0,10) < randomEmptyRange;
        if (!isEmpty) {
            // bool isEnergy = Random.Range();
            int energyAmount = 0;
            bool isEnergyRich = Random.Range(0,10) > 8;
            if (isEnergyRich) {
                energyAmount = Random.Range(6,randomEnergyAmountRange+1);
            } else {
                energyAmount = Random.Range(1,6);
            } 
            blockController.changeEnergy(energyAmount);
        }
        blockController.name = "Block_" + x + "_" + y;
    } 

    void changeSprite(Sprite newSprite) {
        spriteRenderer.sprite = newSprite;
    }
}