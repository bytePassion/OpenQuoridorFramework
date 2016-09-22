using System;
using OQF.Bot.Contracts.Coordination;
using OQF.Bot.Contracts.GameElements;

namespace OQF.GameEngine.Transitions
{
	public static class PlayerTransitions
	{
		public static PlayerState InitialPlayerStateCreation(Player player)
		{
			switch (player.PlayerType)
			{
				case PlayerType.TopPlayer:    return new PlayerState(player, new FieldCoordinate(XField.E, YField.Nine), 10);
				case PlayerType.BottomPlayer: return new PlayerState(player, new FieldCoordinate(XField.E, YField.One ), 10);
			}	
			
			throw new Exception();		
		}

		public static PlayerState MovePlayer(this PlayerState playerState, FieldCoordinate newPosition)
		{
			return new PlayerState(playerState.Player, 
								   newPosition, 				
								   playerState.WallsToPlace);
		}

		public static PlayerState SpendWall(this PlayerState playerState)
		{
			return new PlayerState(playerState.Player,
								   playerState.Position,								   
								   playerState.WallsToPlace-1);
		}
	}
}