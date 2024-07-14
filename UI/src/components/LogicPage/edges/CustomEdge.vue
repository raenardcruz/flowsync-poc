<template>
    <BaseEdge :id="id" :style="style" :path="path[0]" :marker-end="markerEnd" />
    <EdgeLabelRenderer>
        <div :style="{
            pointerEvents: 'all',
            position: 'absolute',
            transform: `translate(-50%, -50%) translate(${path[1]}px,${path[2]}px)`,
        }" class="nodrag nopan">
            <button @click="removeEdges(id)" v-if="!tab.runMode">
                <span class="material-symbols-outlined">cancel</span>
            </button>
        </div>
    </EdgeLabelRenderer>
</template>

<script setup>
import { BaseEdge, EdgeLabelRenderer, getBezierPath, useVueFlow, useEdge } from '@vue-flow/core'
import { computed } from 'vue'
import store from "../../store"

const { edge } = useEdge();
const {tabs} = store();
var tab = computed(() => tabs.value.find(f => f.id == edge.tabId));

const props = defineProps({
    id: {
        type: String,
        required: true,
    },
    sourceX: {
        type: Number,
        required: true,
    },
    sourceY: {
        type: Number,
        required: true,
    },
    targetX: {
        type: Number,
        required: true,
    },
    targetY: {
        type: Number,
        required: true,
    },
    sourcePosition: {
        type: String,
        required: true,
    },
    targetPosition: {
        type: String,
        required: true,
    },
    markerEnd: {
        type: String,
        required: false,
    },
    style: {
        type: Object,
        required: false,
    },
})

const { removeEdges } = useVueFlow()

const path = computed(() => getBezierPath(props))
</script>

<style scoped>
button {
    color: red;
    background: none;
    border: none;
    cursor: pointer;
    transition: 0.5s;
    width: 15px;
    height: 15px;
}
button:hover::before {
    content: 'remove';
    position: absolute;
    color: #fff;
    font-size: 10px;
    top: -10px;
    padding: 3px;
    background: #222;
    border-radius: 20px;
}
</style>