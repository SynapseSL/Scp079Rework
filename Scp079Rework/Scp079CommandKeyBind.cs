using System.Collections.Generic;
using MEC;
using Ninject;
using PlayerRoles;
using Synapse3.SynapseModule;
using Synapse3.SynapseModule.KeyBind;
using Synapse3.SynapseModule.Player;
using UnityEngine;

namespace Scp079Rework;

public abstract class Scp079CommandKeyBind : Scp079Command , IKeyBind
{
    protected Scp079CommandKeyBind() => Timing.RunCoroutine(AwaitMeta());
    
    [Inject]
    public Scp079Rework Module { get; set; }

    private IEnumerator<float> AwaitMeta()
    {
        yield return Timing.WaitUntilTrue(() => Meta != null);
        if (Meta is not Scp079CommandKeyBindAttribute meta) yield break;
        Synapse.Get<KeyBindService>().RegisterKey(this, new KeyBindAttribute()
        {
            CommandName = "Scp079Rework-" + meta.CommandName,
            CommandDescription = meta.Description,
            Bind = meta.DefaultKey
        });
    }

    public virtual bool PreExecuteBind(SynapsePlayer scp079)
    {
        if (scp079.RoleType != RoleTypeId.Scp079) return false;
        if (Time.time < CurrentCooldown && !scp079.Bypass) return false;
        if (GetRequiredLevel(Module) > scp079.MainScpController.Scp079.Level && !scp079.Bypass) return false;
        if (GetRequiredEnergy(Module) > scp079.MainScpController.Scp079.Energy && !scp079.Bypass) return false;
        return true;
    }

    public virtual void AfterExecution(SynapsePlayer scp079)
    {
        if (!scp079.Bypass)
        {
            scp079.MainScpController.Scp079.Energy -= GetRequiredEnergy(Module);
            CurrentCooldown = Time.time + GetCooldown(Module);
        }

        var exp = GetExperienceGain(Module);
        if (exp > 0)
            scp079.MainScpController.Scp079.GiveExperience(exp);
    }
    
    public void Execute(SynapsePlayer scp079)
    {
        if(!PreExecuteBind(scp079)) return;
        if(!ExecuteBind(scp079)) return;
        AfterExecution(scp079);
    }

    public abstract bool ExecuteBind(SynapsePlayer scp079);

    public void Load() { }

    public KeyBindAttribute Attribute { get; set; }
}