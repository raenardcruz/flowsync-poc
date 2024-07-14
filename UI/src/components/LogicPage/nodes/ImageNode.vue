<template>
    <node-template type="text" :id="id">
        <div class="input">
            <label for="value">Value</label>
            <NodeTextbox id="value" :node="node" class="textbox" v-model="node.data.value" ></NodeTextbox>
        </div>
        <div class="input" v-if="tab.runMode">
            <label for="output">Output</label>
            <img class="img-output" :src="log?.output" alt="My Image"/>
        </div>
    </node-template>
    <Handle id="input" type="target" :position="Position.Left" />
    <Handle id="output" type="source" :position="Position.Right" />
</template>

<script setup>
import NodeTextbox from "../components/NodeTextbox.vue"
import { Handle, Position, useNode } from '@vue-flow/core'
import NodeTemplate from "./NodeTemplate.vue"
import store from "../../store"
import { computed } from "vue";

const props = defineProps(['id'])
const { node } = useNode()
const { tabs } = store()
var tab = tabs.value.find(f => f.id == node.tabId);
var log = computed(() => tab.logging.find(f => f.stepId == props.id));
</script>

<style scoped>
@import "./inputStyle.css";
.img-output {
    height: 60px;
    object-fit: contain;
    animation: entry 0.3s linear;
}
</style>