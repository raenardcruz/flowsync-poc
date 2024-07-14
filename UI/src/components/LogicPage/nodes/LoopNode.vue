<template>
    <node-template type="Loop" :id="id">
        <div class="input">
            <label for="times">Iteration</label>
            <NodeTextbox :node="node" id="times" v-model="node.data.times"></NodeTextbox>
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
const { node } = useNode();
</script>

<style scoped>
@import "./inputStyle.css"
</style>