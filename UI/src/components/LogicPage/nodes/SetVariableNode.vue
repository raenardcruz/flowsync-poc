<template>
    <node-template type="Set Variable" :id="id">
        <div class="input">
            <label for="variable">Variable</label>
            <VTextbox :node="node" id="variable" v-model="node.data.name"></VTextbox>
        </div>
        <div class="input">
            <label for="value">Value</label>
            <NodeTextbox id="value" :node="node" class="textbox" v-model="node.data.value" />
        </div>
    </node-template>
    <Handle id="input" type="target" :position="Position.Left" />
    <Handle id="output" type="source" :position="Position.Right" />
</template>

<script setup>
import NodeTemplate from "./NodeTemplate.vue"
import { Handle, Position, useNode } from '@vue-flow/core'
import NodeTextbox from "../components/NodeTextbox.vue"
import VTextbox from "../components/VTextbox.vue"

const props = defineProps(['id'])
const { node } = useNode()
</script>

<style scoped>
@import "./inputStyle.css"
</style>