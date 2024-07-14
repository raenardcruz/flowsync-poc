import store from "../store";

const {
    activeTab,
    tabs } = store();

const closeTab = function (index, tabId) {
    tabs.value.splice(index, 1);
    if (activeTab.value == tabId)
        activeTab.value = 'main';
}
const selectTab = function (tabName) {
    activeTab.value = tabName
}

export {
    closeTab,
    selectTab
}