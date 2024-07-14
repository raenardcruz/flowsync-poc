<template>
    <node-template type="log" :id="id">
        <div class="input">
            <label for="message">Message</label>
            <NodeTextbox :node="node" id="message" v-model="node.data.message"></NodeTextbox> 
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