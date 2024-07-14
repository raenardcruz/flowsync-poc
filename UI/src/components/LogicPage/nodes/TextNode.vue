<template>
    <node-template type="text" :id="id">
        <div class="input">
            <label for="message">Message</label>
            <NodeTextarea :node="node" id="message" v-model="node.data.message" rows="4" cols="30"></NodeTextarea> 
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
import NodeTextarea from "../components/NodeTextarea.vue"
import VTextbox from "../components/VTextbox.vue"

const props = defineProps(['id'])
const { node } = useNode()
</script>

<style scoped>
@import "./inputStyle.css"
</style>