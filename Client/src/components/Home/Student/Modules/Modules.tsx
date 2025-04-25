import ReactMarkdown from "react-markdown";
import "./Modules.css";
import { useEffect, useState } from "react";

function Modules() {
    const [markdownContent, setMarkdownContent] = useState<string>("");

    useEffect(() => {
        fetch("/README.md")
            .then((response) => response.text())
            .then((text) => setMarkdownContent(text))
            .catch((error) => console.error("Erreur lors du chargement du fichier Markdown :", error));
    }, []);

    return (
        <div className="modules">
            <div className="sidebar">
                <div className="modules-list">
                    <h4>Modules</h4>
                    <div>Module 1</div>
                    <div>Module 2</div>
                    <div>Module 3</div>
                    <div>Module 4</div>
                    <div>Module 5</div>
                    <div>Module 6</div>
                    <div>Module 7</div>
                    <div>Module 8</div>
                </div>
            </div>
            <div className="modules-content">
                <h1>Modules</h1>
                <ReactMarkdown>{markdownContent}</ReactMarkdown>
                <div className="eval-button">
                    Start evaluation
                </div>
            </div>
        </div>
    );
}

export default Modules;