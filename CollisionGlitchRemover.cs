﻿using System.Collections;
using HutongGames.PlayMaker.Actions;
using Modding;

namespace CollisionGlitchRemover {
    public class CollisionGlitchRemover : Mod {
        public static CollisionGlitchRemover instance;

        public CollisionGlitchRemover() : base("Collision Glitch Remover") => instance = this;

        public override string GetVersion() => GetType().Assembly.GetName().Version.ToString();

        public bool ToggleButtonInsideMenu => false;

        public override void Initialize() {
            Log("Initializing");

            On.HutongGames.PlayMaker.Actions.CheckCollisionSide.OnExit += OnCheckCollisionSideExit;

            Log("Initialized");
        }

        /*
        The CheckCollisionSide action does not properly reset itself after leaving the midair state at times,
        so the next time the midair state is entered, it immediately triggers the collision transition instead of
        waiting for an actual collision.

        Credit to shownyoung for finding this bug.
        */
        public void OnCheckCollisionSideExit(On.HutongGames.PlayMaker.Actions.CheckCollisionSide.orig_OnExit orig, CheckCollisionSide self) {
            orig(self);

            GameManager.instance.StartCoroutine(ClearCollisions(self));
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
