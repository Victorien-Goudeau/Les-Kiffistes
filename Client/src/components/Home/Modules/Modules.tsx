import { Route, Routes } from "react-router-dom";
import Evaluation from "./Evaluation/Evaluation";
import Module from "./Module/Module";

function Modules() {
    return (
        <Routes>
            <Route path="/" element={<Module />} />
            <Route path="eval" element={<Evaluation />} />
        </Routes>
    );
}

export default Modules;