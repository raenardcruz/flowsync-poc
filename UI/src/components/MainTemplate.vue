<template>
    <div class="page">
        <div class="header">
            <span class="material-symbols-outlined menu">menu</span>
            <span class="material-symbols-outlined" style="color: #3990C5">all_inclusive</span>
            <span class="logo-text" style="color: #3990C5; font-weight: 700;">Flow</span>
            <span class="logo-text">Sync&nbsp;</span>
            <span class="logo-sub-text">STUDIO</span>
        </div>
        <slot class="content"></slot>
    </div>
</template>

<style scoped>
.page {
    position: relative;
    display: grid;
    grid-template-rows: 50px auto;
}
.header {
    position: relative;
    display: flex;
    justify-content: left;
    align-items: center;
    height: 50px;
    background-color: #fff;
    z-index: 10;
    padding-left: 20px;
    box-shadow: 0px 0px 10px grey;
}
.content {
    position: relative;
    display: flex;
    height: 100%;
}
.logo-text {
    font-size: 15px;
}
.logo-sub-text {
    font-size: 11px;
    color: #ADAEAF;
    transform: translateY(2px);
}
.menu {
    position: relative;
    display: flex;
    justify-content: center;
    align-items: center;
    cursor: pointer;
    height: 30px;
    width: 30px;
    padding: 3px;
    border-radius: 90px;
    transition: 0.3s;
    margin-right: 5px;
}
.menu:hover {
    background-color: #ADAEAF;
}
</style>