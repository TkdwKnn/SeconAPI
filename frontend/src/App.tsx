import './App.css'
import RouterAuth from "./routers/router-auth.tsx";
import RouterMain from "./routers/router-main.tsx";

function App() {

    return (
        <div>
            <RouterAuth sourceDir={"/auth"}/>
            <RouterMain sourceDir={"/"}/>
        </div>
    )
}

export default App
