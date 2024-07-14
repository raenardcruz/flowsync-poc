<template>
    <div class="field">
        <div class="checkbox-container" :class="{'selected': isChecked}" @click="toggle">
        <div class="circle" :class="{'selected': isChecked}">
            <span class="material-symbols-outlined" v-if="isChecked" style="color: green;">check</span>
            <span class="material-symbols-outlined" v-else style="color: red;">close</span>
        </div>
    </div>
    <span>{{ label }}</span>
    </div>
</template>

<script setup>
import { defineProps, defineEmits, computed } from 'vue';

const props = defineProps({
  modelValue: Boolean,
  label: String
});

const emit = defineEmits(['update:modelValue']);

const isChecked = computed({
  get: () => props.modelValue,
  set: (value) => emit('update:modelValue', value)
});

const toggle = () => {
  isChecked.value = !isChecked.value;
};
</script>

<style scoped>
    .field {
        position: relative;
        display: flex;
        width: 100%;
    }
    .field span {
        font-size: 15px;
        margin: 0 20px 0 20px;
    }
    .checkbox-container {
        position: relative;
        display: flex;
        align-items: center;
        height: 20px;
        width: 40px;
        border-radius: 20px;
        background: #f0f0f0;
        box-shadow: inset 5px 5px 10px #606060,
            inset -5px -5px 10px #ffffff;
        overflow: hidden;
        cursor: pointer;
    }
    .circle {
        position: absolute;
        display: flex;
        justify-content: center;
        align-items: center;
        height: 18px;
        width: 18px;
        left: 0px;
        border-radius: 100%;
        transition: 0.5s;
        background: #f0f0f0;
        box-shadow:  5px 5px 10px #606060,
             -5px -5px 10px #ffffff;
    }
    .selected.checkbox-container {
        background: #3990C5;
        box-shadow: inset 5px 5px 10px #173a4f,
            inset -5px -5px 10px #5be6ff;
    }
    .selected.circle {
        left: 52%;
        box-shadow:  5px 5px 10px #606060,
        -3px -3px 20px #ffffff;
    }
    .material-symbols-outlined {
        font-size: 10px;
    }
</style>