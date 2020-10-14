pipeline {
	agent any
	stages {
		stage('Clone Repo') {
			steps {
				checkout scm
			}
		}
		stage('Build Image') {
			steps {
				script {
					dockerImage = "test" + ":$BUILD_NUMBER"
				}
			}
		}
