﻿using Ogar_CSharp.sockets;
using Ogar_CSharp.worlds;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ogar_CSharp.bots
{
    public class Minion : Bot
    {
        public Connection following;
        public Minion(Connection following) : base(following.Player.world)
        {
            this.following = following;
            following.minions.Add(this);
        }
        public override string Type => "minion";
        public override bool SeparateInTeams => false;
        public override void Close()
        {
            base.Close();
            following.minions.Remove(this);
        }
        public override void Update()
        {
            if (Player.state == PlayerState.Idle && following.Player.state == PlayerState.Alive)
            {
                spawningName = ((listener.Settings.minionName == "*") ? $"*{following.Player.leaderBoardName}" : listener.Settings.minionName);
                OnSpawnRequest();
                spawningName = null;
            }
            mouseX = following.minionsFrozen ? Player.viewArea.x : following.mouseX;
            mouseY = following.minionsFrozen ? Player.viewArea.y : following.mouseY;
        }
        public override bool ShouldClose => !hasPlayer 
            || !Player.exists 
            || !Player.hasWorld || following.socketDisconnected 
            || following.disconnected || !following.hasPlayer || !following.Player.exists 
            || following.Player.world != Player.world;
    }
}
