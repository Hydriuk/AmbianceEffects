using AmbianceEffects.API;
using AmbianceEffects.Models;
using Cysharp.Threading.Tasks;
using OpenMod.API.Commands;
using OpenMod.Core.Commands;
using OpenMod.Unturned.Commands;
using OpenMod.Unturned.Users;
using SDG.Unturned;
using System;
using System.Linq;
using UHighlight.Models;

namespace AmbianceEffects.OpenMod.Commands
{
    [CommandParent(typeof(AmbianceZoneCommand))]
    [Command("addeffect")]
    [CommandAlias("add")]
    [CommandAlias("a")]
    [CommandSyntax("<zoneName> <effectName> <effectGuid> { OnEnter | OnExit } [<radius>] [-owneronly] [-repeat <minRepeat> <maxRepeat>]")]
    [CommandDescription("Add a new effect to the given zone")]
    [CommandActor(typeof(UnturnedUser))]
    internal class AddEffectCommand : UnturnedCommand
    {
        private readonly IAmbianceStore _zoneStore;
        private readonly IAmbianceSpawner _ambianceSpawner;

        public AddEffectCommand(IServiceProvider serviceProvider, IAmbianceStore ambianceStore, IAmbianceSpawner ambianceSpawner) : base(serviceProvider)
        {
            _zoneStore = ambianceStore;
            _ambianceSpawner = ambianceSpawner;
        }

        protected override async UniTask OnExecuteAsync()
        {
            UnturnedUser user = (UnturnedUser)Context.Actor;

            if (Context.Parameters.Count < 4)
                throw new CommandWrongUsageException(Context);

            string zoneName = Context.Parameters[0];
            string effectName = Context.Parameters[1];

            if (!Guid.TryParse(Context.Parameters[2], out Guid effectGuid) || Assets.find(effectGuid) == null)
                throw new CommandWrongUsageException("effectGuid is not a valid guid");

            if (!Enum.TryParse(Context.Parameters[3], out ZoneProperty.EEvent zoneEvent))
                throw new CommandWrongUsageException($"Could not parse 4th argument. It must be {ZoneProperty.EEvent.OnEnter} or {ZoneProperty.EEvent.OnExit}");

            float radius = 256f;
            bool owneronly = false;
            float minRepeat = -1f;
            float maxRepeat = -1f;
            for (int i = 4; i < Math.Min(Context.Parameters.Count, 9); i++)
            {
                string parameter = Context.Parameters[i];

                if (parameter == "-owneronly")
                {
                    owneronly = true;
                }
                else if (parameter == "-repeat")
                {
                    minRepeat = await Context.Parameters.GetAsync<float>(++i);
                    maxRepeat = await Context.Parameters.GetAsync<float>(++i);

                    if (minRepeat < 0 || maxRepeat <= 0)
                    {
                        throw new UserFriendlyException("minRepeat must be >= 0 and maxRepeat must be > 0");
                    }
                }
                else
                {
                    radius = await Context.Parameters.GetAsync<int>(i);
                }
            }

            AmbianceZone zone = _zoneStore.Get(zoneName);

            if (zone.Effects.Any(effect => effect.Name == effectName))
                throw new UserFriendlyException($"Name {effectName} is already used");

            zone.Effects.Add(new AmbianceEffect()
            {
                Name = effectName,
                Position = user.Player.Player.transform.position,
                EffectGUID = effectGuid,
                Event = zoneEvent,
                VisibiltyRadius = radius,
                OwnerOnly = owneronly,
                MinRepeat = minRepeat,
                MaxRepeat = maxRepeat
            });

            _zoneStore.Update(zone);

            await _ambianceSpawner.ReloadZones();
        }
    }
}