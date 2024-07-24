import { findTab } from "./tabFunctions";
import signalRConnection from "../../scripts/signalrService";

export default class SignalREvents {
    constructor(id) {
        this.tab = findTab(id)
    }

    initializeEvents = () => {
        signalRConnection.on("stepProgress", (runID, stepId, status) => {
            if (this.tab.id == runID) {
              if (this.tab.statuses.filter(f => f.id == stepId).length > 0) {
                this.tab.statuses.filter(f => f.id == stepId)[0].status = status
              } else {
                this.tab.statuses.push({
                  id: stepId,
                  status: status
                })
              }
            }
        });
        signalRConnection.on("log", (log) => {
            if (this.tab.id == log.runId) {
              this.tab.logging.push(log);
            }
        });
        signalRConnection.on("logPath", (runId, source, target) => {
          if (this.tab.id == runId) {
            this.tab.logPaths.push({
              target: target,
              source: source
            });
          }
      });
        signalRConnection.on("processComplete", (runID) => {
            if (this.tab.id == runID) {
              this.tab.runComplete = true;
            }
        });
    }
}