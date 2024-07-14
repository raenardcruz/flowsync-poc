<template>
    <div class="math-node">
        <div class="help no-scroll nodrag" :class="{'show': showHelp}">
            <div class="close" @click="showHelp = false">
                <span class="material-symbols-outlined">close</span>
            </div>
            <div class="content">
                <div class="title">Arithmetic</div>
                <span><legend>`+`</legend> : Addition</span>
                <span><legend>`-`</legend> : Subtraction</span>
                <span><legend>`*`</legend> : Multiplication</span>
                <span><legend>`/`</legend> : Division</span>
                <span><legend>`%`</legend> : Modulus</span>
                <div class="title">Comparison Operators</div>
                <span><legend>`==`</legend> : Equal to</span>
                <span><legend>`!=`</legend> : Not equal to</span>
                <span><legend>`>`</legend> : Greater than</span>
                <span><legend>`<`</legend> : Less than</span>
                <span><legend>`>=`</legend> : Greater than or equal to</span>
                <span><legend>`<=`</legend> : less than or equal to</span>
                <div class="title">Logical Operators</div>
                <span><legend>`&&`</legend> : Logical AND</span>
                <span><legend>`||`</legend> : Logical OR</span>
                <span><legend>`!`</legend> : Logical NOT</span>
                <div class="title">Functions</div>
                <span><legend>`Abs(x)`</legend> : Absolute value</span>
                <span><legend>`Acos(x)`</legend> : Arc cosine</span>
                <span><legend>`Asin(x)`</legend> : Arc sine</span>
                <span><legend>`Atan(x)`</legend> : Arc tangent</span>
                <span><legend>`Ceiling(x)`</legend> : Ceiling</span>
                <span><legend>`Cos(x)`</legend> : Cosine</span>
                <span><legend>`Exp(x)`</legend> : Exponential</span>
                <span><legend>`Floor(x)`</legend> : Floor</span>
                <span><legend>`IEEERemainder(3, 2)`</legend> : Returns the remainder resulting from the division of a specified number by another specified number.</span>
                <span><legend>`Log(x, y)`</legend> : Natural logarithm</span>
                <span><legend>`Log10(x)`</legend> : Base 10 logarithm</span>
                <span><legend>`Max(x, y)`</legend> : Base 10 logarithm</span>
                <span><legend>`Min(x, y)`</legend> : Base 10 logarithm</span>
                <span><legend>`Pow(x, y)`</legend> : Power</span>
                <span><legend>`Round(x, decimals)`</legend> : Round to specified number of decimal places</span>
                <span><legend>`Sign(x)`</legend> : Signum</span>
                <span><legend>`Sin(x)`</legend> : Sine</span>
                <span><legend>`Sqrt(x)`</legend> : Square root</span>
                <span><legend>`Tan(x)`</legend> : Tangent</span>
                <span><legend>`Truncate(x)`</legend> : Truncate</span>
                <span><legend>`in(value, listvalue1, listvalue2, ...)`</legend> : Returns whether an element is in a set of values.</span>
                <span><legend>`if(logic, trueValue, falseValue)`</legend> : Returns a value based on a condition.</span>
                <span><legend>`ifs(logic1, trueValue, logic2, trueValue, falseValue)`</legend> : Returns a value based on evaluating a number of conditions, returning a default if none are true.</span>
            </div>
        </div>
        <node-template type="math" :id="id">
            <template #actions>
                <button class="help-btn" @click="showHelp = !showHelp" style="color: red;">
                    <span class="material-symbols-outlined">help</span>
                </button>
            </template>
            <div class="input">
                <label for="expression">Expression</label>
                <NodeTextbox :node="node" id="expression" v-model="node.data.expression"></NodeTextbox>
            </div>
            <div class="input">
                <label for="variable">Variable</label>
                <VTextbox :node="node" id="variable" v-model="node.data.variable"></VTextbox>
            </div>
        </node-template>
        <Handle id="input" type="target" :position="Position.Left" />
        <Handle id="output" type="source" :position="Position.Right" />
    </div>
</template>

<script setup>
import { ref } from "vue"
import NodeTemplate from "./NodeTemplate.vue"
import { Handle, Position, useNode } from '@vue-flow/core'
import NodeTextbox from "../components/NodeTextbox.vue"
import VTextbox from "../components/VTextbox.vue"

const props = defineProps(['id'])
const { node } = useNode()
const showHelp = ref(false);
</script>

<style scoped>
@import "./inputStyle.css";

.math-node {
    position: relative;
    display: flex;
}
.math-node .help {
    position: absolute;
    display: flex;
    flex-wrap: wrap;
    flex-direction: column;
    height: 5px;
    justify-content: center;
    width: 5px;
    background: #f0f0f0;
    border-radius: 10px;
    right: 50%;
    top: 50%;
    opacity: 0;;
    cursor: auto;
    overflow: auto;
    user-select: text;
    z-index: 100;
    transition: 0.3s;
    box-shadow:  6px 6px 12px #868686,
             -6px -6px 12px #ffffff;
}
.math-node .help.show {
    width: 300px;
    right: -320px;
    top: -100px;
    height: fit-content;
    opacity: 1;
}
.math-node .help .close {
    position: absolute;
    right: 10px;
    top: 10px;
    color: red;
    cursor: pointer;
    z-index: 100;
}
.math-node .help .title {
    position: relative;
    font-size: 15px;
    width: 100%;
    margin: 10px 0 10px 0;
}
.math-node .help .content {
    position: relative;
    display: flex;
    flex-direction: column;
    justify-content: left;
    font-size: 10px;
    width: 100%;
    height: 300px;
    padding: 10px;
    text-wrap: nowrap;
}
.math-node .help .content span {
    position: relative;
    display: flex;
    width: 100%;
    flex-wrap: wrap;
    text-wrap: pretty;
    word-wrap: break-word;
}
legend {
    font-weight: 600;
}
.help-btn {
    border: none;
    cursor: pointer;
}
</style>