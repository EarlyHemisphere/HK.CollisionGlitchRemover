using System.Collections;
using HutongGames.PlayMaker.Actions;
using Modding;

namespace CollisionGlitchRemover {
    public class CollisionGlitchRemover : Mod, ITogglableMod {
        public static CollisionGlitchRemover instance;
        public bool enabled = false;

        public CollisionGlitchRemover() : base("Collision Glitch Remover") => instance = this;

        public override string GetVersion() => GetType().Assembly.GetName().Version.ToString();

        public override void Initialize() {
            Log("Initializing");

            enabled = true;
            On.HutongGames.PlayMaker.Actions.CheckCollisionSide.OnExit += OnCheckCollisionSideExit;

            Log("Initialized");
        }

        public void Unload() => enabled = false;

        /*
        The CheckCollisionSide action does not properly reset itself after leaving the midair state at times,
        so the next time the midair state is entered, it immediately triggers the collision transition instead of
        waiting for an actual collision.

        Credit to shownyoung for finding the cause of this bug.
        */
        public void OnCheckCollisionSideExit(On.HutongGames.PlayMaker.Actions.CheckCollisionSide.orig_OnExit orig, CheckCollisionSide self)
        {
            orig(self);

            if (enabled) {
                GameManager.instance.StartCoroutine(ClearCollisions(self));
            }
        }

        public IEnumerator ClearCollisions(CheckCollisionSide instance) {
            yield return null; // Wait for one frame to ensure values are no longer being updated

            instance.topHit = false;
            instance.rightHit = false;
            instance.bottomHit = false;
            instance.leftHit = false;
        }
    }
}
