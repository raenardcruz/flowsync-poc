<template>
    <node-template type="Condition" :id="id">
        <div class="input">
            <label for="expression">Expression</label>
            <NodeTextbox :node="node" id="expression" v-model="node.data.expression"></NodeTextbox>
        </div>
    </node-template>
    <Handle id="input" type="target" :position="Position.Left" />
    <Handle id="outputTrue" class="output-true" type="source" :position="Position.Right" />
    <Handle id="outputFalse" class="output-false" type="source" :position="Position.Right" />
</template>

<script setup>
import NodeTemplate from "./NodeTemplate.vue"
import { Handle, Position, useNode } from '@vue-flow/core'
import NodeTextbox from "../components/NodeTextbox.vue"

const props = defineProps(['id'])
const { node } = useNode()
</script>

<style scoped>
@import "./inputStyle.css";

.output-true {
    top: 40%
}

.output-true::before {
    content: 'True';
    font-size: 7px;
    position: absolute;
    inset: -8px 8px;
}

.output-false {
    top: 60%
}

.output-false::before {
    content: 'False';
    font-size: 7px;
    position: absolute;
    inset: -8px 8px;
}
#expression {
    field-sizing: content;
}
</style>