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

    public int[] getPostition() {
        return new int[] { (int)(transform.position.x - 0.5f), (int)(transform.position.y - 0.5f) };
    }

    public int getLevel() {
        // lv0 0
        // lv1 1 - 14 
        // lv2 15 - 29
        // lv3 30 -
        if (energyAmount == 0 && magicAmount == 0) 
            return 0;
        else if(Mathf.Max(energyAmount, magicAmount) < 44)
            return Mathf.Max(energyAmount, magicAmount)/15 + 1;
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

    void changeSprite(Sprite newSprite) {
        spriteRenderer.sprite = newSprite;
    }
}