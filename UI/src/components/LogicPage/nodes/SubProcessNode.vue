<template>
    <node-template type="subprocess" :id="id">
        <div class="input">
            <label for="message">Process</label>
            <select v-model="node.data.value">
                <option v-for="process in filteredProcesses" :key="process.id" :value="process.id">{{ process.title }}</option>
            </select>
        </div>
        <template v-slot:test>
            <div class="view-process" @click="viewProcess">View Process</div>
        </template>
    </node-template>
    <Handle id="input" type="target" :position="Position.Left" />
    <Handle id="output" type="source" :position="Position.Right" />
</template>

<script setup>
import NodeTemplate from "./NodeTemplate.vue"
import { Handle, Position, useNode } from '@vue-flow/core'
import store from "../../store";
import { computed, onMounted } from "vue";
import { selectTab } from "../scripts/processFunctions";

const { processes, tabs } = store()
const props = defineProps(['id'])
const { node } = useNode()
const filteredProcesses = computed(() => {
    return processes.value.filter(f => f.id != node.tabId && f.type == "default")
})
var tab = tabs.value.find(f => f.id == node.tabId)
const viewProcess = function () {
    selectTab(node.data.value, true, tab.logging, tab.statuses, tab.logPaths)
}
</script>

<style scoped>
@import "./inputStyle.css";
.view-process {
    position: relative;
    display: flex;
    justify-content: center;
    align-items: center;
    padding: 3px;
    border: 1px solid #fff;
    margin-top: 10px;
    border-radius: 20px;
    box-shadow: 2px 2px 4px grey;
    transition: all 0.3s ease-in-out;
}
.view-process:hover {
    background: rgba(18, 166, 219, 0.871);
    border: none;
    box-shadow: 0 0 3px rgba(18, 166, 219, 0.871), 0 0 6px rgba(18, 166, 219, 0.871), 0 0 10px rgba(18, 166, 219, 0.871);
}
</style>