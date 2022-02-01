#include <windows.h>
#include <shellapi.h>
#include <curl/curl.h>
#include <thread>
#include "Dota2Patcher.h"

size_t WriteRemoteString(void* ptr, size_t size, size_t nmemb, void* stream) {
	std::string data((const char*)ptr, (size_t)size * nmemb);
	*((std::stringstream*)stream) << data;
	return size * nmemb;
}

bool UpdateRequired() {
	std::stringstream out;
	CURL* curl = curl_easy_init();
	curl_easy_setopt(curl, CURLOPT_URL, "https://raw.githubusercontent.com/Wolf49406/Dota2Patcher/master/version.txt");
	curl_easy_setopt(curl, CURLOPT_WRITEFUNCTION, WriteRemoteString);
	curl_easy_setopt(curl, CURLOPT_WRITEDATA, &out);
	CURLcode CURLresult = curl_easy_perform(curl);
	std::string remote_version = out.str();

	if (strcmp(remote_version.c_str(), Globals::local_version.c_str()))
		return true;

	return false;
}

bool Patcher::CheckUpdate() {
	if (!UpdateRequired())
		return false;

	ShellExecuteA(0, 0, "https://github.com/Wolf49406/Dota2Patcher/releases/latest", 0, 0, SW_SHOW);
	return true;
}
