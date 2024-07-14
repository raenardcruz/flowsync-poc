import { findTab } from "./tabFunctions";
import signalRConnection from "../../scripts/signalrService";

export default class SignalREvents {
    constructor(id) {
        this.tab = findTab(id)
    }

    initializeEvents = () => {
        signalRConnection.on("stepProgress", (runID, stepId, status) => {
            if (this.tab.id == runID) {
              var noderesult = this.tab.nodes.filter(f => f.id == stepId);
              if (noderesult.length > 0) {
                var node = noderesult[0];
                node.data.status = status
              }
            }
        });
        signalRConnection.on("log", (log) => {
            if (this.tab.id == log.runId) {
              this.tab.logging.push(log);
            }
        });
        signalRConnection.on("processComplete", (runID) => {
            if (this.tab.id == runID) {
              this.tab.runComplete = true;
            }
        });
    }
}