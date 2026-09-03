window.taskTimerUnload = {

    handler: null,

    register: function (taskId, apiUrl) {

        this.unregister();

        this.handler = function (event) {

            // F5 / browser refresh detect
            const navigationEntries =
                performance.getEntriesByType("navigation");

            if (navigationEntries.length > 0) {

                const navigationType =
                    navigationEntries[0].type;

                // Refresh ko ignore karein
                if (navigationType === "reload") {
                    return;
                }
            }

            const token =
                sessionStorage.getItem("authToken");

            const payload = JSON.stringify({
                taskId: taskId
            });

            fetch(apiUrl, {
                method: "POST",

                headers: {
                    "Content-Type": "application/json",
                    "Authorization": token
                        ? "Bearer " + token
                        : ""
                },

                body: payload,

                keepalive: true
            }).catch(function () {
                // Browser close ke waqt errors ignore
            });
        };

        window.addEventListener(
            "pagehide",
            this.handler
        );
    },

    unregister: function () {

        if (this.handler) {

            window.removeEventListener(
                "pagehide",
                this.handler
            );

            this.handler = null;
        }
    }
};