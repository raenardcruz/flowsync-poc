<template>
    <node-template type="While" :id="id">
        <div class="input">
            <label for="limit">Limit</label>
            <NodeTextbox :node="node" id="limit" v-model="node.data.limit"></NodeTextbox>
        </div>
        <div class="input">
            <label for="expression">Expression</label>
            <NodeTextbox :node="node" id="expression" v-model="node.data.expression"></NodeTextbox>
        </div>
    </node-template>
    <Handle id="input" type="target" :position="Position.Left" />
    <Handle id="output" type="source" :position="Position.Right" />
</template>

<script setup>
import NodeTemplate from "./NodeTemplate.vue"
import { Handle, Position, useNode } from '@vue-flow/core'
import NodeTextbox from "../components/NodeTextbox.vue"

const props = defineProps(['id'])
const { node } = useNode()
</script>

<style scoped>
@import "./inputStyle.css"
</style>