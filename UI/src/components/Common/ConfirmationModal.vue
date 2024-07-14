<template>
    <div class="confirmation-bg" v-if="show">
        <div class="confirmation-card">
            <div class="title">{{ title }}</div>
            <div class="message">{{ message }}</div>
            <div class="actions">
                <button class="btn confirm" @click="isOk = true">Confirm</button>
                <button class="btn cancel" @click="show = false">Cancel</button>
            </div>
        </div>
    </div>
</template>

<script setup>
import { ref, watch, defineExpose } from "vue"
const message = ref("")
const show = ref(false)
const title = ref("Confirmation")
const isOk = ref(false)
const showModal = (ttle, msg) => {
    title.value = ttle
    message.value = msg
    isOk.value = false;
    show.value = true;
    return new Promise(resolve => {
        watch(isOk, (newVal) => {
            if (newVal) {
                show.value = false
                resolve(newVal)
            }
        })
        watch(show, (newVal) => {
            if (!newVal) {
                show.value = false
                resolve(newVal)
            }
        })
    })
}
defineExpose({
    showModal
})
</script>

<style scoped>
.confirmation-bg {
    position: absolute;
    display: flex;
    justify-content: center;
    align-items: center;
    background: rgba(0, 0, 0, 0.342);
    height: 100%;
    width: 100%;
    z-index: 100;
}
.confirmation-card {
    position: relative;
    display: flex;
    flex-direction: column;
    width: fit-content;
    min-width: 300px;
    height: fit-content;
    max-height: 500px;
    background: #fff;
    border-radius: 8px;
    padding: 20px;
    animation: entry 0.3s
}
.title {
    font-size: 25px;
    font-weight: 600;
}
.message {
    font-size: 15px;
    margin-top: 20px;
    color: rgb(109, 109, 109);
}
.confirm {
    background: #3990C5;
}
.cancel {
    background: rgb(198, 111, 58);
}
.btn {
    border: none;
    padding: 10px;
    margin: 10px;
    border-radius: 8px;
    cursor: pointer;
    transition: 0.5s;
    color: #fff;
}
.btn:hover {
    filter: brightness(1.2);
}
</style>