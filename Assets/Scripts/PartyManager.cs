using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    [SerializeField] private PartyMemberInfo[] allMembers;
    [SerializeField] private List<PartyMember> currentParty;
    [SerializeField] private PartyMemberInfo defaultPartyMember;

    private void Awake(){
        AddMemberToPartyByName(defaultPartyMember.MemberName);
    }
    public void AddMemberToPartyByName(string memberName)
    {
        for(int i=0; i<allMembers.Length; i++){
            if(allMembers[i].MemberName == memberName){
                PartyMember newPartyMember = new PartyMember();
                newPartyMember.Level = allMembers[i].StartingLevel;
                newPartyMember.CurrHealth = allMembers[i].BaseHealth;
                newPartyMember.MaxHealth = newPartyMember.CurrHealth;
                newPartyMember.Strength = allMembers[i].BaseStr;
                newPartyMember.Initiative = allMembers[i].BaseInitiative;
                newPartyMember.MemberOverworldVisualPrefab = allMembers[i].MemberOverworldVisualPrefab;
                newPartyMember.MemberBattleVisualPrefab = allMembers[i].MemberBattleVisualPrefab;

                currentParty.Add(newPartyMember);
            }
        }
    }
}


[System.Serializable]
public class PartyMember
{
    public string MemberName;
    public int Level;
    public int MaxHealth;
    public int CurrHealth;
    public int Strength;
    public int Initiative;
    public int CurrExp;
    public int MaxExp;
    public GameObject MemberBattleVisualPrefab;      //What will be displayed in battle scene
    public GameObject MemberOverworldVisualPrefab;
}
