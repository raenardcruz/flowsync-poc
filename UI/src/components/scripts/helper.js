const copyObj = function (obj) {
    return JSON.parse(JSON.stringify(obj))
}

export {
    copyObj
}