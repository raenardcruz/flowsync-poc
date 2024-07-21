import store from "../../store.js"
import { decrypt, copyObj } from "../../scripts/functions.js"
import api from "./api.js"
import defaultTab from "./defaultTabConstant.js"
import startNodeTemplate from "./startNodeTemplateConstant.js"
import { v4 as uuidv4 } from 'uuid'

const { processes, tabs, activeTab, tags } = store() 

const getAllProcesses = function () {
    processes.value = []
    api.getAllProcess()
    .then(response => {
        var decryptedListStr = decrypt(response.data, import.meta.env.VITE_SECRET);
        var decryptedList = JSON.parse(decryptedListStr);
        decryptedList.forEach(process => {
            processes.value.push(process);
        })
        tags.value = [...new Set(processes.value.map(m => m.tags).flat().filter(f => f != null))].map(m => {
            return {
                name: m,
                value: true
            }
        })
        tags.value.push({
            name: "No Tags",
            value: true
        })
        tags.value = tags.value.sort((a, b) => a.name.toLowerCase().localeCompare(b.name.toLowerCase()))
    })
    .catch(err => alert(err))
}

const selectTab = function (tabId, runMode = false, logging = []) {
    if (tabs.value.findIndex(f => f.id == tabId) > -1) {
        activeTab.value = tabId;
        return;
    }
    api.getProcess(tabId)
    .then(response => {
        var decryptedStr = decrypt(response.data, import.meta.env.VITE_SECRET);
        var decrypted = JSON.parse(decryptedStr);
        var tabTemplate = copyObj(defaultTab);
        for (let key in decrypted) {
            if (tabTemplate.hasOwnProperty(key)) {
                if (decrypted[key] != null)
                    tabTemplate[key] = decrypted[key];
            }
        }
        tabTemplate.isNew = false;
        var vueFlowObj = JSON.parse(decrypted.components);
        vueFlowObj.nodes.filter(f => f.id == "1")[0] = copyObj(startNodeTemplate);
        tabTemplate.nodes = vueFlowObj.nodes?.length > 0 ? [...vueFlowObj.nodes] : [];
        tabTemplate.edges = vueFlowObj.edges?.length > 0 ? [...vueFlowObj.edges] : [];
        tabTemplate.variableProfiles = vueFlowObj.variableProfile?.length > 0 ? [...vueFlowObj.variableProfile] : [];
        if (runMode) {
            tabTemplate.runMode = true;
            tabTemplate.runComplete = true;
            tabTemplate.logging = logging;
        }
        tabs.value.push(tabTemplate);
        activeTab.value = tabTemplate.id;
    })
    .catch(err => alert(err))
}

const newProcess = function () {
    var id = uuidv4();
    var startNode =  copyObj(startNodeTemplate);
    startNode.tabId = id;
    var newTab = copyObj(defaultTab);
    newTab.id = id;
    newTab.nodes.push(startNode);
    tabs.value.push(newTab);
    activeTab.value = id;
}

export {
    getAllProcesses,
    selectTab,
    newProcess
}