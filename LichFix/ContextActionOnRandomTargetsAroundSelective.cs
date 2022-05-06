using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Abilities;

namespace LichFix
{
    [TypeId("2dcc60a5b076f0047b982e9d4c04b221")]
    public class ContextActionOnRandomTargetsAroundSelective:ContextActionOnRandomTargetsAround
    {
        public override void RunAction()
        {

            var hasSelective = base.Context.SourceAbilityContext.HasMetamagic(Metamagic.Selective);

            Main.Log("ContextActionOnRandomTargetsAroundSelective is run! hasSelective = "+hasSelective);

            if (hasSelective){
                this.OnEnemies = true;
            }
            else
            {
                this.OnEnemies = false;
            }

            base.RunAction();
        }

        public ContextActionOnRandomTargetsAroundSelective(){}
    }
}
