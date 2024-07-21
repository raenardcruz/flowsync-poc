const nodeGroups = [
    {
        icon: "handyman",
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
            "regexfind",
            "subprocess"
        ]
    },
    {
        icon: "network_node",
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
        icon: "transform",
        name: "Data Transformation",
        description: "Transforms data formats and structures, including mapping, parsing, and aggregating data.",
        members: [
            "map"
        ]
    },
    {
        icon: "cable",
        name: "Connectivity",
        description: "Connects to external systems and services, including messaging, APIs, and software connectors.",
        members: [
            "api"
        ]
    },
    {
        icon: "security",
        name: "Security",
        description: "Handles authentication, encryption, and security-related operations.",
        members: []
    },
    {
        icon: "analytics",
        name: "Monitoring and Analytics",
        description: "Collects metrics, monitors performance, and generates reports and dashboards.",
        members: [
            "log"
        ]
    },
    {
        icon: "output",
        name: "Output",
        description: "Output nodes are used for testing visualization. These nodes are not executed in deployed process.",
        members: [
            "image"
        ]
    }
]

export default nodeGroups;