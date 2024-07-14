import store from "../../store";

const { tabs } = store();
const findTab = function (id) {
    return tabs.value.find(f => f.id == id);
}

export {
    findTab
}