using osu.Framework.Testing;

namespace Chess.Game.Tests.Visual
{
    public abstract partial class ChessTestScene : TestScene
    {
        protected override ITestSceneTestRunner CreateRunner() => new ChessTestSceneTestRunner();

        private partial class ChessTestSceneTestRunner : ChessGameBase, ITestSceneTestRunner
        {
            private TestSceneTestRunner.TestRunner runner;

            protected override void LoadAsyncComplete()
            {
                base.LoadAsyncComplete();
                Add(runner = new TestSceneTestRunner.TestRunner());
            }

            public void RunTestBlocking(TestScene test) => runner.RunTestBlocking(test);
        }
    }
}
