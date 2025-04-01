class BindData {
    constructor(containerId, data) {
        this.containerId = containerId;
        this.containerElement = document.getElementById(containerId);
        this.elements = {}; 
        this.data = this.createReactiveData(data); 
    }

  
    initialize() {
        if (!this.containerElement) {
            console.error(`Container with ID '${this.containerId}' not found.`);
            return;
        }

  
        this.containerElement.querySelectorAll("[id]").forEach(el => {
            this.elements[el.id] = el;

  
            if (el.tagName === "INPUT" || el.tagName === "TEXTAREA" || el.tagName === "SELECT") {
                el.addEventListener("input", () => {
                    this.setNestedValue(this.data, el.id, el.value);
                });
            }
        });

        this.updateDOM();
    }

  
    createReactiveData(data) {
        return new Proxy(data, {
            set: (target, key, value) => {
                target[key] = value;
                this.updateDOM(); // Auto-update UI
                return true;
            }
        });
    }

  
    setNestedValue(obj, path, value) {
        let keys = path.split(".");
        let lastKey = keys.pop();
        let nestedObj = keys.reduce((o, key) => (o[key] = o[key] || {}), obj);
        nestedObj[lastKey] = value;
    }

  
    getNestedValue(obj, path) {
        return path.split(".").reduce((o, key) => (o ? o[key] : ""), obj);
    }

  
    updateDOM() {
        for (let key in this.elements) {
            let element = this.elements[key];
            let value = this.getNestedValue(this.data, key);

            if (element.tagName === "INPUT" || element.tagName === "TEXTAREA" || element.tagName === "SELECT") {
                element.value = value || "";
            } else {
                element.innerText = value || "";
            }
        }
    }

  
    updateData(newData) {
        Object.keys(newData).forEach(key => {
            this.setNestedValue(this.data, key, newData[key]);
        });
        this.updateDOM(); // Ensure UI updates
    }
}
