using System.Collections;
using System;
using System.Collections.Generic;

namespace TreeSharpPlus
{

	public class probNode : Node {

		private static Random RANDOM = new Random();

		private double probability;
		private Node child1;
		private Node child2;
		private Node child;

		protected Node lastTicked = null;

		public override Node LastTicked {
			get { return lastTicked; }
		}

		public probNode(Node child1, Node child2, double probability) {
			this.child1 = child1;
			this.child2 = child2;
			this.probability = probability;
		}

		public override void Stop() {
			if (lastTicked != null) {
				lastTicked.Stop();
			}
			base.Stop();
		}

		public override RunStatus Terminate() {
			RunStatus curStatus = StartTermination();
			if (curStatus != RunStatus.Running)
				return curStatus;

			// If a node active then end it
			if (lastTicked != null) {
				return ReturnTermination(lastTicked.Terminate());
			}
			return ReturnTermination(RunStatus.Success);
		}

		public override void ClearLastStatus() {
			this.LastStatus = null;
			child.ClearLastStatus();
		}

		public override IEnumerable<RunStatus> Execute() {
			if (RANDOM.NextDouble() <= probability) {
				child = child1;
			} else {
				child = child2;
			}
			lastTicked = child;

			child.Start();
			RunStatus result;
			while ((result = child.Tick()) == RunStatus.Running) {
				yield return RunStatus.Running;
			}
			child.Stop();

			yield return result;
			yield break;
		}
	}
}