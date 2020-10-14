pipeline {
	environment {
		registry = "registry.kmalinowski.net/TheGreatSpillTracker"
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
					dockerImage = docker.build registry + ":$BUILD_NUMBER"
				}
			}
		}
		stage('Push Image') {
			steps {
				script {
					dockerImage.push()
				}
			}
		}
		stage('Cleanup') {
			steps {
				sh "docker rmi $registy:$BUILD_NUMBER"
			}
		}
	}
}
