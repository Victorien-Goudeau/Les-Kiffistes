import "./Evaluation.css";

function Evaluation() {
    
    return (
        <div className="evaluation-module">
            <h1>Evaluation</h1>
            <div className="question-container">
                <div className="question-container-header">
                    <h2>Question 1</h2>
                </div>
                <div className="question-container-body">
                    <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua ?</p>
                    <div className="responses-container">
                        <div className="response-option">
                            <input type="radio" id="option1" name="question1" value="option1" />
                            <label htmlFor="option1" className="option-label">Option 1</label>
                        </div>
                        <div className="response-option">
                            <input type="radio" id="option2" name="question1" value="option2" />
                            <label htmlFor="option2" className="option-label">Option 2</label>
                        </div>
                        <div className="response-option">
                            <input type="radio" id="option3" name="question1" value="option3" />
                            <label htmlFor="option3" className="option-label">Option 3</label>
                        </div>
                        <div className="response-option">
                            <input type="radio" id="option4" name="question1" value="option4" />
                            <label htmlFor="option4" className="option-label">Option 4</label>
                        </div>
                        <div className="response-option">
                            <input type="radio" id="option5" name="question1" value="option5" />
                            <label htmlFor="option5" className="option-label">Option 5</label>
                        </div>
                        <div className="response-option">
                            <input type="radio" id="option6" name="question1" value="option6" />
                            <label htmlFor="option6" className="option-label">Option 6</label>
                        </div>
                    </div>
                </div>
                <div className="question-container-footer">
                    <button className="submit-button">Next</button>
                </div>
            </div>
        </div>
    );
}

export default Evaluation;