using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSModel;
using ZRTSMapEditor.MapEditorModel;

namespace ZRTSMapEditor
{
    public class ChangeScenarioContextVisitor : NoOpMapEditorModelVisitor
    {
        private ScenarioComponent scenario = null;
        private ScenarioComponent prevScenario;
        private bool scenarioChanged = false;

        public bool ScenarioChanged
        {
            get { return scenarioChanged; }
            set { scenarioChanged = value; }
        }
 
        public void SetPrevScenario(ScenarioComponent scenario)
        {
            prevScenario = scenario;
        }

        public ScenarioComponent GetScenario()
        {
            return scenario;
        }

        public override void Visit(MapEditorFullModel model)
        {
            scenario = model.GetScenario();
            if (scenario != prevScenario)
            {
                scenarioChanged = true;
            }
            base.Visit(model);
        }
    }
}
