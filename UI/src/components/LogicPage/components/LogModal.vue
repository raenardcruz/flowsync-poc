<template>
    <div class="background">
        <div class="modal">
            <div class="modal-header">
                <span class="material-symbols-outlined close-btn" @click="tab.showLogModal = false">close</span>
                <h4 style="margin-bottom: 20px;">Logs</h4>
            </div>
            <div class="modal-content">
                <table>
                    <tr>
                        <th>Execution Date</th>
                        <th>Step Id</th>
                        <th>Step</th>
                        <th>Messages</th>
                    </tr>
                    <tr v-for="log in tab.logging.sort((a, b) => a.stepCount - b.stepCount)" :key="log">
                        <td>{{ utcDateToLocal(log.dateTime) }}</td>
                        <td>{{ log.stepId }}</td>
                        <td>
                            <span>{{log.label}}</span>
                            <span>({{ log.type }})</span>
                        </td>
                        <td>
                            <ul>
                                <li v-for="logmessage in log.messages" :key="logmessage">
                                    <span v-html="displayText(logmessage)"></span>
                                </li>
                            </ul>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</template>

<script setup>
import store from "../../store.js";

const props = defineProps(['id'])
const { tabs } = store();
var tab = tabs.value.filter(f => f.id == props.id)[0];
const utcDateToLocal = function (utcDate) {
    const date = new Date(utcDate);
    const year = date.getFullYear();
    const month = ('0' + (date.getMonth() + 1)).slice(-2);
    const day = ('0' + date.getDate()).slice(-2);
    const hours = ('0' + date.getHours()).slice(-2);
    const minutes = ('0' + date.getMinutes()).slice(-2);
    const seconds = ('0' + date.getSeconds()).slice(-2);
    const localDate = `${month}/${day}/${year} ${hours}:${minutes}:${seconds}`;
    
    return localDate;
}

const displayText = function (text) {
    if (text == null)
        return "";
    if (text.length > 300) {
        const blob = new Blob([text], { type: 'text/plain' });
        var url = URL.createObjectURL(blob);
        return `Content too big. Click <a href="${url}" target="_blank">HERE</a> to view the whole document`
    } else {
        return text;
    }
}
</script>

<style scoped>
.background {
    position: absolute;
    display: flex;
    height: 100%;
    width: 100%;
    justify-content: center;
    align-items: center;
    background: rgba(0, 0, 0, 0.525);
    z-index: 100;
}
.background .modal {
    position: relative;
    display: flex;
    flex-direction: column;
    height: 50vh;
    width: 60vw;
    min-height: 100px;
    min-width: 300px;
    border-radius: 10px;
    background: #fff;
    padding: 20px;
    overflow: auto;
    animation: modalentry 0.5s;
    user-select: text;
}
.background .modal .modal-header {
    position: relative;
    display: flex;
    justify-content: center;
    height: 50px;
}
.background .modal .modal-content {
    position: relative;
    display: flex;
    overflow: auto;
}
table {
    width: fit-content !important;
    min-width: 100%;
    overflow: hidden;
    border-collapse: collapse;
}
th,
th {
    background: #dddddd;
}

td,
th {
    border: 0.5px solid #dddddd;
    text-align: center;
    font-size: 13px;
    min-width: 60px;
}
.close-btn {
    position: absolute;
    right: 10px;
    top: 10px;
    color: red;
    cursor: pointer;
    transition: 0.3s;
}
.close-btn:hover {
    transform: scale(1.05);
}
ul {
    list-style-type: none;
    padding: 0;
    margin: 0;
}
@keyframes modalentry {
    from {
        transform: translateY(20px);
    } 
    to {
        transform: translateY(0px);
    }
}
</style>