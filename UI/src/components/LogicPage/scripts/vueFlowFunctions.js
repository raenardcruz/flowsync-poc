import api from "./api";
import { getAllProcesses } from "./processFunctions";
import store from "../../store";
import { copyObj } from "../../scripts/helper";

const { processes, activeTab, tabs } = store()

const save = (tab, notify) => {
    var payload = copyObj(tab);
    console.log(tab.edges)
    payload.components = JSON.stringify({
        nodes: tab.nodes,
        edges: tab.edges,
        variableProfile: tab.variableProfiles
    })
    var headers = {
      headers: {
        'Content-Type': 'application/json',
        'runId': tab.id
      }
    }
    api.upsertProcess(headers, payload)
      .then(() => {
        tab.isNew = false;
        getAllProcesses();
        notify.show(`Process Saved`);
      })
      .catch(err => {
        notify.show(err, "error");
      })
}

const deleteProcess = (tab, confirmation, notify) => {
    confirmation.showModal("Delete Confirmation", `Are you sure you want to delete ${tab.name}?`)
      .then(response => {
        if (response) {
          api.deleteProcess(tab.id)
            .then(() => {
              getAllProcesses();
              processes.value.splice(processes.value.findIndex(f => f.id == tab.id), 1)
              activeTab.value = 'main'
              tabs.value.splice(tabs.value.findIndex(f => f.id == tab.id), 1)
            })
            .catch(err => {
              notify.show(err, "error");
            })
        }
      })
  }

const quickrun = (tab, notify) => {
    tab.runMode = true;
    tab.logging = [];
    tab.logPaths = [];

    tab.nodes.filter(f => f.id != "1").forEach(node => node.data.status = "")
    var payload = {
        nodes: tab.nodes,
        edges: tab.edges
    };
    var headers = {
        headers: {
            'Content-Type': 'application/json',
            'runId': tab.id
        }
    }
    api.quickRun(headers, payload)
        .then(response => notify.show(response.data))
        .catch(err => notify.show(err, "error"))
}

const exitRunMode = (tab) => {
    tab.statuses = [];
    tab.runMode = false;
}
  

  export {
    deleteProcess,
    quickrun,
    exitRunMode,
    save
  }