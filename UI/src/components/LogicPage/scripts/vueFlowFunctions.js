import api from "./api";
import { getAllProcesses } from "./processFunctions";
import store from "../../store";

const { processes, activeTab, tabs } = store()

const deleteProcess = function (tab, confirmation, notify) {
    confirmation.value.showModal("Delete Confirmation", `Are you sure you want to delete ${tab.name}?`)
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
              notify.value.show(err, "error");
            })
        }
      })
  }

const quickrun = function (tab, notify) {
    tab.runMode = true;
    tab.logging = [];
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
    api.quickRun(headers, encrypt(JSON.stringify(payload), import.meta.env.VITE_SECRET))
        .then(response => notify.value.show(response.data))
        .catch(err => notify.value.show(err, "error"))
}

const exitRunMode = function (tab) {
    tab.nodes.filter(f => f.id != "1").forEach(node => node.data.status = "");
    tab.runMode = false;
}
  

  export {
    deleteProcess,
    quickrun,
    exitRunMode
  }