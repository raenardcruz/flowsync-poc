<template>
    <div class="custom-node" :id="id" :class="(node.selected ? 'node-selected ' : ' ') + nodeStatus" @click="nodeClicked" @mouseenter="nodeHover" @mouseleave="showLog = false">
        <div class="status" v-if="nodeStatus.length > 0">
            <span class="material-symbols-outlined running" v-if="nodeStatus == 'running'">cached</span>
            <span class="material-symbols-outlined success" v-if="nodeStatus == 'success'">check</span>
            <span class="material-symbols-outlined failed" v-if="nodeStatus == 'failed'">error</span>
        </div>
        <div class="header">
            <div class="title">
                <div class="label">
                    <div class="icon" :style="'color:' + iconColor(node.type).color">
                        <span class="material-symbols-outlined">{{ iconColor(node.type).icon }}</span>
                    </div>
                    <input type="text" v-model="node.label" v-if="edit" />
                    <span v-else>
                        <span v-if="node.label.length > 0">{{ node.label }}</span>
                        <span v-else>{{ type }}</span>
                    </span>
                    <div class="edit-btn" @click="save">
                        <span class="material-symbols-outlined" v-if="edit" style="font-size: 8px;">save</span>
                        <span class="material-symbols-outlined" v-else style="font-size: 8px;">edit</span>
                    </div>
                    <slot name="actions"></slot>
                </div>
                <div class="type">
                    <span v-if="node.label.length > 0">{{ type }}</span>
                </div>
                <div class="type">{{ node.id }}</div>
            </div>
        </div>
        <div class="node-content nodrag no-scroll" :class="{disabled: tab.runMode}">
            <slot></slot>
        </div>
        <div class="log-content" v-if="showLog">
            <h4>Incomming Data</h4>
            <div v-html="displayText(log?.input)"></div>
            <br>
            <h4>Outgoing Data</h4>
            <div v-html="displayText(log?.output)"></div>
            <br>
            <h4>Logs</h4>
            <div v-for="message in log?.messages" :key="message">
                <div v-html="displayText(message)"></div>
            </div>
            <slot name="test"></slot>
        </div>
    </div>
</template>

<script setup>
import store from "../../store"
import { computed, ref } from "vue"
import { useVueFlow, useNode } from '@vue-flow/core'
import { iconStyles } from "../scripts/constants.js";

const {
    onNodeDragStop,
    removeSelectedNodes,
    addSelectedNodes,
    getSelectedNodes
} = useVueFlow()
const { node } = useNode()
const edit = ref(false);
const showLog = ref(false);
const props = defineProps(['id', 'type'])
const { tabs } = store()
var tab = tabs.value.find(f => f.id == node.tabId)
var log = computed(() => tab.logging.find(f => f.stepId == props.id));
var nodeStatus = computed(() => {
    var statusResults = tab.statuses.filter(f => f.id == node.id);
    if (statusResults.length > 0) {
        return statusResults[0].status;
    } else
        return ""
})

onNodeDragStop((event) => {
    if (event.node.id == props.id) {
        tab.nodes.filter(f => f.id == node.id)[0].position = event.node.position;
    }
})
const nodeClicked = () => {
    removeSelectedNodes(getSelectedNodes.value);
    addSelectedNodes([node]);
}
const nodeHover = () => {
    if (tab.runMode)
        showLog.value = true
}
const iconColor = (type) => {
    var filterStyle = iconStyles.filter(f => f.name == type);
    if (filterStyle.length > 0)
        return filterStyle[0];
    else
        return {
            icon: "square",
            color: "#fff"
        }
}
const displayText = (text) => {
    if (text == null)
        return "";
    if (text.length > 300) {
        const blob = new Blob([text], { type: 'text/plain' });
        var url = URL.createObjectURL(blob);
        return `Content too big. Click <a href="${url}" target="_blank">HERE</a> to view the whole document`
    } else {
        return text;
    }
}
const save = () => {
    edit.value = !edit.value;
    tab.nodes.find(f => f.id == node.id).label = node.label;
}
</script>

<style scoped>
.disabled * {
  pointer-events: none;
}
.custom-node {
    position: relative;
    border-radius: 8px;
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    grid-template-rows: 20px auto;
    height: 100%;
    width: fit-content !important;
    max-width: 250px !important;
    cursor: pointer;
    animation: entry 0.3s;
    transition: 0.3s;
    background: #f0f0f0;
    box-shadow:  3px 3px 5px #868686,
             -3px -3px 5px #ffffff;
}
.log-content {
    position: absolute;
    display: flex;
    flex-direction: column;
    padding: 20px;
    background: #000000b2;
    color: #fff;
    z-index: 100;
    font-size: 13px;
    height: fit-content;
    width: fit-content;
    border-radius: 8px;
    overflow: auto;
    animation: entry 0.2s linear;
}
.custom-node:hover {
    transform: scale(1.02);
    box-shadow:  6px 6px 12px #868686,
             -6px -6px 12px #ffffff;
}
.custom-node .status {
    position: absolute;
    display: flex;
    justify-content: center;
    align-items: center;
    height: 30px;
    width: 30px;
    border-radius: 90px;
    z-index: 3;
    left: -10px;
    top: -15px;
    box-shadow: 0px 5px 10px grey;
    overflow: hidden;
}
.custom-node .status .running {
    height: 40px;
    width: 40px;
    color:#0D5CDA;
    background: #fff;
    display: flex;
    justify-content: center;
    align-items: center;
    animation: runningload 1s linear infinite;
}
.custom-node .status .success {
    height: 40px;
    width: 40px;
    color:#28CE83;
    background: #fff;
    display: flex;
    justify-content: center;
    align-items: center;
}
.custom-node .status .failed {
    height: 40px;
    width: 40px;
    color:#CE2831;
    background: #fff;
    display: flex;
    justify-content: center;
    align-items: center;
}
.custom-node .header {
    overflow: hidden;
    min-height: 30px;
    height: fit-content;
    padding: 7px;
    border-radius: 10px 10px 0 0;
    position: relative;
    display: flex;
    justify-content: center;
    align-items: center;
    width: 100%;
    background: #f0f0f0;
    padding: 10px;
}

.custom-node .header * {
    font-size: 10px;
}

.custom-node .header .title {
    position: relative;
    display: flex;
    flex-direction: column;
}

.custom-node .header .title .label {
    position: relative;
    display: flex;
    justify-content: center;
    align-items: center;
}

.custom-node .header .title .label input {
    width: 100%;
    border-radius: 20px;
    padding: 2px;
    font-size: 8px;
    border: 0.05px solid grey;
}

.custom-node .header .title .label input:focus {
    outline: none;
    border: 0.05px solid #1092B3;
}

.custom-node .header .title .type {
    font-size: 7px;
    color: #8E8C8C;
}

.custom-node .header .icon {
    margin-right: 10px;
}

.custom-node .header .edit-btn {
    position: relative;
    display: flex;
    justify-content: center;
    align-items: center;
    align-self: baseline;
    height: 12px;
    width: 12px;
    border-radius: 90px;
    margin: 0 5px 0 5px;
}

.custom-node .header .edit-btn:hover {
    color: #098C92;
    box-shadow: 2px 2px 3px grey;
}

.custom-node .node-content {
    height: fit-content;
    padding: 10px;
    position: relative;
    width: 100%;
}

.custom-node .node-content * {
    width: 100%;
}

.node-selected {
    border: 1px solid #3B90D4;
}

.custom-node.running {
    border: 1px solid #0D5CDA;
}

.custom-node.success {
    border: 1px solid #28CE83;
}

.custom-node.failed {
    border: 1px solid #CE2831;
}
@keyframes animate {
    0% {
        width: 105%;
        height: 105%;
        filter: blur(5px);
    }

    100% {
        width: 110%;
        height: 110%;
        filter: blur(20px);
    }
}
@keyframes runningload {
    0% {
        transform: rotate(0deg);
    }
    100% {
        transform: rotate(360deg);
    }
}
</style>