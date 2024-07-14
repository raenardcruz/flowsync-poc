<template>
    <node-template type="count" :id="id">
        <div class="input">
            <label for="list">List</label>
            <NodeTextbox :node="node" id="list" v-model="node.data.list"></NodeTextbox>
        </div>
        <div class="input">
            <label for="template">Template</label>
            <NodeTextarea :node="node" id="template" v-model="node.data.template" rows="4" cols="30"></NodeTextarea>
        </div>
        <div class="input">
            <label for="variable">Variable</label>
            <VTextbox :node="node" id="variable" v-model="node.data.variable"></VTextbox>
        </div>
    </node-template>
    <Handle id="input" type="target" :position="Position.Left" />
    <Handle id="output" type="source" :position="Position.Right" />
</template>

<script setup>
import NodeTemplate from "./NodeTemplate.vue"
import { Handle, Position, useNode } from '@vue-flow/core'
import NodeTextbox from "../components/NodeTextbox.vue"
import NodeTextarea from "../components/NodeTextarea.vue"
import VTextbox from "../components/VTextbox.vue"

const props = defineProps(['id'])
const { node } = useNode();
</script>

<style scoped>
@import "./inputStyle.css"
</style>