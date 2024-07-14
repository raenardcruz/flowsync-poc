const nodeGroups = [
    {
        name: "Utility",
        description: "Performs fundamental operations like math, string manipulations, date operations, variable management, text replacement, and regex operations.",
        members: [
            "setVariable",
            "getGuid",
            "text",
            "math",
            "list",
            "count",
            "replace",
            "regexfind"
        ]
    },
    {
        name: "Logic/Control Flow",
        description: "Manages workflow logic with conditions, loops, and delays.",
        members: [
            "condition",
            "loop",
            "foreach",
            "while",

        ]
    },
    {
        name: "Data Transformation",
        description: "Transforms data formats and structures, including mapping, parsing, and aggregating data.",
        members: [
            "map"
        ]
    },
    {
        name: "Connectivity",
        description: "Connects to external systems and services, including messaging, APIs, and software connectors.",
        members: [
            "api"
        ]
    },
    {
        name: "Security",
        description: "Handles authentication, encryption, and security-related operations.",
        members: []
    },
    {
        name: "Monitoring and Analytics",
        description: "Collects metrics, monitors performance, and generates reports and dashboards.",
        members: [
            "log"
        ]
    },
    {
        name: "Output",
        description: "Output nodes are used for testing visualization. These nodes are not executed in deployed process.",
        members: [
            "image"
        ]
    }
]

export default nodeGroups;