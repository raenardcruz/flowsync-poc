<template>
    <BaseEdge :id="id" :style="edgeStyling" :path="path[0]" :marker-end="markerEnd" />
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
import { BaseEdge, EdgeLabelRenderer, getSmoothStepPath, useVueFlow, useEdge } from '@vue-flow/core'
import { computed } from 'vue'
import store from "../../store"

const { edge } = useEdge();
const { tabs } = store();

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

var tab = computed(() => tabs.value.find(f => f.id == edge.tabId));
var pathRan = computed(() => {
    return tab.value.logPaths.filter(f => f.source == edge.source && f.target == edge.target).length > 0;
})
var edgeStyling = computed(() => {
    if (tab.value.runMode) {
        if (pathRan.value)
            return ({
                ...props.style,
                stroke: 'black',
                strokeWidth: 3
            })
        else
                return ({
                    ...props.style,
                    stroke: '#C0BFBF',
                    strokeWidth: 1
                })
    } else
        return ({
                    ...props.style,
                    stroke: 'grey'
                })
})
const path = computed(() => getSmoothStepPath({
    sourceX: props.sourceX,
    sourceY: props.sourceY,
    targetX: props.targetX,
    targetY: props.targetY,
    sourcePosition: props.sourcePosition,
    targetPosition: props.targetPosition,
}))
</script>

<style scoped>
button {
    position: relative;
    display: flex;
    justify-content: center;
    align-items: center;
    color: red;
    background: #fff;
    border: none;
    cursor: pointer;
    transition: all 0.3s ease-in-out;
    width: 15px;
    height: 15px;
    border-radius: 90px;
}
button:hover {
    background: red;
    color: #fff;
    box-shadow: 0 0 4px red, 0 0 8px red, 0 0 12px red;
}
button span {
    font-size: 20px;
}
</style>
