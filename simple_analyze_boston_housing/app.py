from flask import Flask, render_template, request, jsonify
from flask_cors import CORS
import pandas as pd
from csv import reader

app = Flask(__name__)
CORS(app)

# Carregando os dados
df = pd.read_csv("dados/HousingData.csv")

"""
Rota principal onde renderiza para o 'index.html'
"""
#@app.route("/")
#def index():
#    return render_template("index.html")

"""
Rota para análise com base nos valores informados
e renderiza para a página 'analysis.html'
"""
@app.route("/analyze", methods=["POST"])
def analyze():
    # Obter parâmetros formulário
    feature = request.form.get("feature")
    # Calcula estatísticas
    mean_value = round(df[feature].mean(), 2)
    median_value = round(df[feature].median(),2)
    max_value = round(df[feature].max(),2)
    min_value = round(df[feature].min(),2)

    # return render_template("analysis.html", feature=feature, mean_value=mean_value, median_value=median_value, max_value=max_value, min_value=min_value)
    return jsonify({
        "feature": feature,
        "mean_value": mean_value,
        "median_value": median_value,
        "max_value": max_value,
        "min_value": min_value
    })

@app.route("/base", methods=["GET"])
def base():
    data = df.to_dict(orient="list")        
    return jsonify(data)

if __name__ == "__main__":
    app.run(debug=True)